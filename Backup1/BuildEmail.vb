Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.Data.SqlTypes
Imports Microsoft.SqlServer.Server


Partial Public Class StoredProcedures

    <Microsoft.SqlServer.Server.SqlProcedure()> _
    Public Shared Sub BuildNotificationEmail(incidentID As SqlInt32)
        'Public Sub BuildNotificationEmail(incidentID As SqlInt32)

        Dim errorMsg As String = String.Empty

        Dim incidentDate As Date
        Dim incidentLocation As String
        Dim incidentType As String
        Dim incidentTypeID As Integer
        Dim incidentLevel As Integer
        Dim studentID As String, staffID As String, businessKey As Integer, concequenceID As Integer, concequence As String, concequenceDateStart As Date, concequenceDateEnd As Date, processingRequired As Boolean,
            processedDate As Date, communicationRequired As Boolean, modifiedDate As Date, modifiedBy As String, notes As String

        Dim conn As New System.Data.SqlClient.SqlConnection
        conn.ConnectionString = "Context Connection=true"
        Dim dsIncident As New DataSet

        Try
            Using conn
                SqlContext.Pipe.Send("SELECT FROM Behaviour.Incident")
                '++++++++++++++++++
                Dim adp As New System.Data.SqlClient.SqlDataAdapter("SELECT incidentDate, incidentLocation, incidentTypeID, incidentType, incidentLevel, studentID, staffID, businessKey, concequenceID, concequence, concequenceDateStart, concequenceDateEnd, processingRequired, processedDate, communicationRequired, modifiedDate, modifiedBy, notes FROM  Behaviour.Incident WHERE incidentId=@incidentId", conn)
                adp.SelectCommand.CommandType = CommandType.Text
                '@consequenceID as int
                adp.SelectCommand.Parameters.Add("@incidentID", SqlDbType.Int).Value = incidentID

                conn.Open()
                adp.Fill(dsIncident, "IncidentDetails")
            End Using
            If dsIncident.Tables(0).Rows.Count > 0 Then
                Dim result As Date
                For Each row As DataRow In dsIncident.Tables(0).Rows
                    If Not IsDBNull(row("incidentDate")) Then
                        If DateTime.TryParse(row("incidentDate"), result) Then
                            incidentDate = result
                        Else
                            incidentDate = Today
                        End If
                    End If
                    If Not IsDBNull(row("incidentLocation")) Then
                        incidentLocation = row("incidentLocation")
                    End If
                    If Not IsDBNull(row("incidentTypeID")) Then
                        incidentTypeID = row("incidentTypeID")
                    End If
                    If Not IsDBNull(row("incidentType")) Then
                        incidentType = row("incidentType")
                    End If
                    If Not IsDBNull(row("incidentLevel")) Then
                        incidentLevel = row("incidentLevel")
                    End If
                    If Not IsDBNull(row("studentID")) Then
                        studentID = row("studentID")
                    End If
                    If Not IsDBNull(row("staffID")) Then
                        staffID = row("staffID")
                    End If
                    If Not IsDBNull(row("concequenceID")) Then
                        concequenceID = row("concequenceID")
                    End If
                    If Not IsDBNull(row("concequence")) Then
                        concequence = row("concequence")
                    End If
                    If Not IsDBNull(row("concequenceDateStart")) Then
                        If DateTime.TryParse(row("concequenceDateStart"), result) Then
                            concequenceDateStart = result
                        Else
                            concequenceDateStart = Today
                        End If
                    End If
                    If Not IsDBNull(row("concequenceDateEnd")) Then
                        If DateTime.TryParse(row("concequenceDateEnd"), result) Then
                            concequenceDateEnd = result
                        Else
                            concequenceDateEnd = Today
                        End If
                    End If
                    If Not IsDBNull(row("processingRequired")) Then
                        processingRequired = row("processingRequired")
                    End If
                    If Not IsDBNull(row("processedDate")) Then
                        If DateTime.TryParse(row("processedDate"), result) Then
                            processedDate = result
                        Else
                            processedDate = Today
                        End If
                    End If
                    If Not IsDBNull(row("communicationRequired")) Then
                        communicationRequired = row("communicationRequired")
                    End If
                Next

            Else
                'incidentConsquenceType = "Unknown"
                Throw New System.Exception("Consequence missing in Database error - could not determine whether consequence is processed or need to notify as no data returned from dB")
                errorMsg = "Consequence missing in Database error - could not determine whether consequence is processed or need to notify as no data returned from dB"
            End If
        Catch ex As SqlException
            With ex
                errorMsg = "Error Number: " & .Number
                errorMsg &= " -- Error State: " & .State
                errorMsg &= " -- Error Message: " & .Message
                'incidentConsquenceType = "Unknown"
                'Throw New System.Exception("Database access error - could not determine whether consequence is processed or need to notify.")
                errorMsg &= "Database access error - could not SELECT FROM  Behaviour.Incident. Line 120"
            End With
        Finally
            conn.Close()
            conn.Dispose()
            If Not errorMsg Is String.Empty Then
                Throw New System.Exception(errorMsg)
            End If
        End Try

        'Get Communication Template Data
        Dim dsCommunicationTemplate As New DataSet
        Dim communicationTemplateId As Integer, description As String, subject As String, message As String, sendToStudent As Boolean, sendToParent As Boolean, sendToPCT As Boolean, sendToHOH As Boolean, sendToDP As Boolean
        Try
            conn.ConnectionString = "Context Connection=true"
            Using conn
                Dim adp As New System.Data.SqlClient.SqlDataAdapter("SELECT communicationTemplateId, description, subject, message, sendToStudent, sendToParent, sendToPCT, sendToHOH, sendToDP FROM  Behaviour.ConcequenceCommunicationTemplate WHERE concequenceId=@concequenceId", conn)
                adp.SelectCommand.CommandType = CommandType.Text

                '@consequenceID as int
                adp.SelectCommand.Parameters.Add("@concequenceId", SqlDbType.Int).Value = concequenceID

                conn.Open()
                adp.Fill(dsCommunicationTemplate, "CommunicationTemplate")
            End Using
            If dsCommunicationTemplate.Tables(0).Rows.Count > 0 Then
                For Each row As DataRow In dsCommunicationTemplate.Tables(0).Rows
                    If Not IsDBNull(row("communicationTemplateId")) Then
                        communicationTemplateId = row("communicationTemplateId")
                    End If
                    If Not IsDBNull(row("description")) Then
                        description = row("description")
                    End If
                    If Not IsDBNull(row("subject")) Then
                        subject = row("subject")
                    End If
                    If Not IsDBNull(row("message")) Then
                        message = row("message")
                    End If
                    If Not IsDBNull(row("sendToStudent")) Then
                        sendToStudent = row("sendToStudent")
                    End If
                    If Not IsDBNull(row("sendToParent")) Then
                        sendToParent = row("sendToParent")
                    End If
                    If Not IsDBNull(row("sendToPCT")) Then
                        sendToPCT = row("sendToPCT")
                    End If
                    If Not IsDBNull(row("sendToHOH")) Then
                        sendToHOH = row("sendToHOH")
                    End If
                    If Not IsDBNull(row("sendToDP")) Then
                        sendToDP = row("sendToDP")
                    End If
                Next
            End If
        Catch ex As SqlException
            With ex
                errorMsg = "Error Number: " & .Number
                errorMsg &= " -- Error State: " & .State
                errorMsg &= " -- Error Message: " & .Message
                'incidentConsquenceType = "Unknown"
                'Throw New System.Exception("Database access error - could not determine whether consequence is processed or need to notify.")
                errorMsg &= "Database access error - could not SELECT FROM  Behaviour.ConcequenceCommunicationTemplate - Line 177."
            End With
        Finally
            conn.Close()
            conn.Dispose()
            If Not errorMsg Is String.Empty Then
                Throw New System.Exception(errorMsg)
            End If
        End Try

        'Get Communication Template Data Fields
        Dim dsCommunicationTemplateDataFields As New DataSet
        Dim communicationsTemplateFields As New Collection
        Dim TemplateDataItem As TemplateDataType

        Try
            conn.ConnectionString = "Context Connection=true"
            Using conn
                'SELECT incidentCommunicationTemplateDataId, fieldName, fieldValue FROM  Behaviour.IncidentCommunicationTemplateData WHERE incidentId=@incidentId
                Dim adp As New System.Data.SqlClient.SqlDataAdapter("SELECT incidentCommunicationTemplateDataId, fieldName, fieldValue FROM  Behaviour.IncidentCommunicationTemplateData WHERE incidentId=@incidentId", conn)
                adp.SelectCommand.CommandType = CommandType.Text
                '@incidentId as int
                adp.SelectCommand.Parameters.Add("@incidentId", SqlDbType.Int).Value = incidentID

                conn.Open()
                adp.Fill(dsCommunicationTemplateDataFields, "CommunicationTemplateDataFields")
            End Using
            SqlContext.Pipe.Send("Template Data Field Data Items Count =" & dsCommunicationTemplateDataFields.Tables(0).Rows.Count.ToString)
            If dsCommunicationTemplateDataFields.Tables(0).Rows.Count > 0 Then
                SqlContext.Pipe.Send("Template Data Field Data Items")
                For Each row As DataRow In dsCommunicationTemplateDataFields.Tables(0).Rows
                    If Not IsDBNull(row("fieldName")) Then
                        TemplateDataItem.fieldName = row("fieldName")
                    End If
                    If Not IsDBNull(row("fieldValue")) Then
                        TemplateDataItem.dataItem = row("fieldValue")
                    End If
                    communicationsTemplateFields.Add(TemplateDataItem)

                    SqlContext.Pipe.Send(TemplateDataItem.fieldName.ToString & " " & TemplateDataItem.dataItem.ToString)
                Next
            End If
        Catch
            'incidentConsquenceType = "Unknown"
            'Throw New System.Exception("Database access error - could not determine whether consequence is processed or need to notify.")
            errorMsg = "Database access error - could not SELECT FROM  Behaviour.IncidentCommunicationTemplateData - Line 219."

        Finally
            conn.Close()
            conn.Dispose()
            If Not errorMsg Is String.Empty Then
                Throw New System.Exception(errorMsg)
            End If
        End Try

        SqlContext.Pipe.Send("Declare to variables")
        'Build "to" list
        Dim toList As String = String.Empty
        'Build "cc" list
        Dim ccList As String = String.Empty
        'Build "bcc" list
        Dim bccList As String = String.Empty
        'Build "from" list
        Dim fromList As String = String.Empty

        '++++++++++++++++++++++++++++++++++++++++++++++
        '==== Get Student
        SqlContext.Pipe.Send("Get student data")
        Dim studentData As Collection = getStudentData(studentID)
        SqlContext.Pipe.Send("Insert student data into message")
        For Each studentDataItem As TemplateDataType In studentData
            Dim searchString As String = "@Student" & studentDataItem.fieldName
            message = Replace(message, searchString, studentDataItem.dataItem)
        Next
        If sendToStudent Then
            SqlContext.Pipe.Send("set student sendTo")
            If studentData.Contains("mail") Then
                Dim toValue As TemplateDataType = studentData("mail")
                If toList <> String.Empty Then
                    toList &= ";" & toValue.dataItem.ToString
                Else
                    toList = toValue.dataItem.ToString
                End If
            End If
        End If
        '==== Get Family
        Dim familyData As Collection = getFamilyData(studentID)
        For Each familyDataItem As TemplateDataType In familyData
            Dim searchString As String = "@Family" & familyDataItem.fieldName
            message = Replace(message, searchString, familyDataItem.dataItem)
            SqlContext.Pipe.Send(searchString)
        Next
        If sendToParent Then
            SqlContext.Pipe.Send("############  set family sendTo")
            If familyData.Contains("familyEmail") Then
                Dim familyMail As TemplateDataType = familyData("familyEmail")
                SqlContext.Pipe.Send("Parent Mail")
                SqlContext.Pipe.Send(familyMail.dataItem.ToString)
                If toList <> String.Empty Then
                    toList &= ";" & familyMail.dataItem.ToString
                Else
                    toList = familyMail.dataItem.ToString
                End If
            End If
            '@FamilyfamilyEmail2
            If familyData.Contains("familyEmail2") Then
                Dim familyMail As TemplateDataType = familyData("familyEmail2")
                SqlContext.Pipe.Send("Parent Mail2")
                SqlContext.Pipe.Send(familyMail.dataItem.ToString)
                If toList <> String.Empty Then
                    toList &= ";" & familyMail.dataItem.ToString
                Else
                    toList = familyMail.dataItem.ToString
                End If
            End If
        End If


        '==== Get DP
        Dim deputyPrincipalCollection As New Collection
        If sendToDP Then
            Dim deputyPrincipalIDCollection As Collection = getDPID()
            For Each dpID As String In deputyPrincipalCollection
                SqlContext.Pipe.Send("DP is " & dpID)
                Dim dp As Collection = getStaffData(dpID)
                deputyPrincipalCollection.Add(dp)
                If deputyPrincipalCollection.Count = 1 Then
                    For Each dpDataItem As TemplateDataType In dp
                        Dim searchString As String = "@DP" & dpDataItem.fieldName
                        message = Replace(message, searchString, dpDataItem.dataItem)
                    Next
                End If
            Next
        End If
        If sendToDP Then
            For Each dp As Collection In deputyPrincipalCollection
                If dp.Contains("mail") Then
                    Dim dpMail As TemplateDataType = dp("mail")
                    SqlContext.Pipe.Send("DP Mail")
                    SqlContext.Pipe.Send(dpMail.dataItem.ToString)
                    If ccList <> String.Empty Then
                        ccList &= ";" & dpMail.dataItem.ToString
                    Else
                        ccList = dpMail.dataItem.ToString
                    End If
                End If
            Next
        End If

        '==== Get HOH
        Dim hohID As String
        hohID = getHOHID(studentID)
        Dim hohData As Collection = getStaffData(hohID)
        For Each hohDataItem As TemplateDataType In hohData
            Dim searchString As String = "@HOH" & hohDataItem.fieldName
            message = Replace(message, searchString, hohDataItem.dataItem)
        Next
        If sendToHOH Then
            If hohData.Contains("mail") Then
                Dim hohMail As TemplateDataType = hohData("mail")
                SqlContext.Pipe.Send("HOH Mail")
                SqlContext.Pipe.Send(hohMail.dataItem.ToString)
                If ccList <> String.Empty Then
                    ccList &= ";" & hohMail.dataItem.ToString
                Else
                    ccList = hohMail.dataItem.ToString
                End If
            End If
        End If

        '==== Get Pastoral
        Dim pastorallCollection As New Collection
        If sendToPCT Then
            Dim pastoralIDCollection As Collection = getPastoralID(studentID)
            For Each pastoralID As String In pastoralIDCollection
                SqlContext.Pipe.Send("PCT is " & pastoralID)
                Dim pct As Collection = getStaffData(pastoralID)
                pastorallCollection.Add(pct)
                If pastoralIDCollection.Count = 1 Then
                    For Each pctDataItem As TemplateDataType In pct
                        Dim searchString As String = "@PCT" & pctDataItem.fieldName
                        message = Replace(message, searchString, pctDataItem.dataItem)
                    Next
                End If
            Next
        End If
        If sendToPCT Then
            For Each pct As Collection In pastorallCollection
                SqlContext.Pipe.Send("Number of PCT:" & pastorallCollection.Count.ToString)
                If pct.Contains("mail") Then
                    Dim pctMail As TemplateDataType = pct("mail")
                    SqlContext.Pipe.Send("PCT Mail")
                    SqlContext.Pipe.Send(pctMail.dataItem.ToString)
                    If ccList <> String.Empty Then
                        ccList &= ";" & pctMail.dataItem.ToString
                    Else
                        ccList = pctMail.dataItem.ToString
                    End If
                End If
            Next
        End If
        ''''==============

        ''''=Staff is working use for other staff types=============
        '==== Get Staff
        Dim staffData As Collection = getStaffData(staffID)
        SqlContext.Pipe.Send("Replace Staff template fields")
        For Each staffDataItem As TemplateDataType In staffData
            Dim searchString As String = "@Staff" & staffDataItem.fieldName
            'SqlContext.Pipe.Send(searchString)
            message = Replace(message, searchString, staffDataItem.dataItem)
        Next
        If staffData.Contains("mail") Then
            Dim staffMail As TemplateDataType = staffData("mail")
            SqlContext.Pipe.Send("Staff Mail")
            SqlContext.Pipe.Send(staffMail.dataItem.ToString)
            If fromList <> String.Empty Then
                fromList &= ";" & staffMail.dataItem.ToString
            Else
                fromList = staffMail.dataItem.ToString
            End If
            If ccList <> String.Empty Then
                ccList &= ";" & staffMail.dataItem.ToString
            Else
                ccList = staffMail.dataItem.ToString
            End If
        End If

        'Build email Message
        For Each dataItem As TemplateDataType In communicationsTemplateFields
            Dim searchString As String = "@" & dataItem.fieldName
            SqlContext.Pipe.Send("Message - Replace " & searchString)
            Dim pos As Integer = InStr(message, searchString, CompareMethod.Binary)
            SqlContext.Pipe.Send("        - Pos " & pos)
            message = Replace(message, searchString, dataItem.dataItem) ', , , CompareMethod.Binary)
        Next

        Dim sent As SqlDateTime = Now
        Dim communicationType As String = "Notification"

        Dim cmd As SqlCommand = New SqlCommand("INSERT INTO [DataCentral].[Behaviour].[IncidentCommunication]([incidentId],[sent],[from],[to],[cc],[bcc],[message],[subject],[communicationType]) VALUES(@incidentId, @sent, @from, @to,@cc,@bcc,@message,@subject,@communicationType)", conn)
        cmd.CommandType = CommandType.Text
        cmd.Parameters.Add(New SqlParameter("@incidentId", SqlDbType.Int)).Value = incidentID
        cmd.Parameters.Add(New SqlParameter("@sent", SqlDbType.SmallDateTime)).Value = sent
        cmd.Parameters.Add(New SqlParameter("@from", SqlDbType.VarChar, 255)).Value = fromList
        cmd.Parameters.Add(New SqlParameter("@to", SqlDbType.VarChar, 255)).Value = toList
        cmd.Parameters.Add(New SqlParameter("@cc", SqlDbType.VarChar, -1)).Value = ccList
        cmd.Parameters.Add(New SqlParameter("@bcc", SqlDbType.VarChar, -1)).Value = bccList
        cmd.Parameters.Add(New SqlParameter("@message", SqlDbType.VarChar, -1)).Value = message
        cmd.Parameters.Add(New SqlParameter("@subject", SqlDbType.VarChar, 100)).Value = subject
        cmd.Parameters.Add(New SqlParameter("@communicationType", SqlDbType.VarChar, 50)).Value = communicationType

        conn.ConnectionString = "Context Connection=true"
        conn.Open()
        Try
            cmd.ExecuteNonQuery()
        Catch
            'incidentConsquenceType = "Unknown"
            'Throw New System.Exception("Database access error - could not determine whether consequence is processed or need to notify.")
            errorMsg = "Database access error - could not INSERT INTO [DataCentral].[Behaviour].[IncidentCommunication] - Line 263."
        Finally
            conn.Close()
            conn.Dispose()
            If Not errorMsg Is String.Empty Then
                Throw New System.Exception(errorMsg)
            End If
        End Try

    End Sub

    Private Shared Function getPrincipalID() As String
        Dim principalID As String = String.Empty
        Dim errorMsg As String = String.Empty

        Dim conn As New System.Data.SqlClient.SqlConnection
        Dim dsPrincipalData As New DataSet

        Try
            conn.ConnectionString = "Context Connection=true"
            Using conn
                Dim adp As New System.Data.SqlClient.SqlDataAdapter("SELECT EmployeeID FROM  DC_StudentManagement_Organisation WHERE (roleClassification = 'Principal')", conn)
                adp.SelectCommand.CommandType = CommandType.Text

                conn.Open()
                adp.Fill(dsPrincipalData, "principalData")
            End Using
            If dsPrincipalData.Tables(0).Rows.Count > 0 Then
                For Each row As DataRow In dsPrincipalData.Tables(0).Rows
                    If Not IsDBNull(row("EmployeeID")) Then
                        principalID = row("EmployeeID").ToString
                    End If
                Next
            End If
        Catch ex As SqlException
            With ex
                errorMsg = "Error Number: " & .Number
                errorMsg &= " -- Error State: " & .State
                errorMsg &= " -- Error Message: " & .Message
                'incidentConsquenceType = "Unknown"
                'Throw New System.Exception("Database access error - could not determine whether consequence is processed or need to notify.")
                errorMsg &= "Database access error - could not SELECT FROM  DC_Student - getStudentData Function."
            End With
        Finally
            conn.Close()
            conn.Dispose()
            If Not errorMsg Is String.Empty Then
                Throw New System.Exception(errorMsg)
            End If
        End Try

        Return principalID
    End Function

    Private Shared Function getDPID() As Collection
        Dim DPList As New Collection

        Dim DPID As String = String.Empty
        Dim errorMsg As String = String.Empty

        Dim conn As New System.Data.SqlClient.SqlConnection
        Dim dsDPData As New DataSet

        Try
            conn.ConnectionString = "Context Connection=true"
            Using conn
                Dim adp As New System.Data.SqlClient.SqlDataAdapter("SELECT EmployeeID FROM  DC_StudentManagement_Organisation WHERE (roleClassification = 'Deputy Principal') AND (roleIdentifier = 'Students')", conn)
                adp.SelectCommand.CommandType = CommandType.Text

                conn.Open()
                adp.Fill(dsDPData, "DPData")
            End Using
            If dsDPData.Tables(0).Rows.Count > 0 Then
                For Each row As DataRow In dsDPData.Tables(0).Rows
                    If Not IsDBNull(row("EmployeeID")) Then
                        DPID = row("EmployeeID").ToString
                        DPList.Add(DPID, DPID)
                    End If
                Next
            End If
        Catch ex As SqlException
            With ex
                errorMsg = "Error Number: " & .Number
                errorMsg &= " -- Error State: " & .State
                errorMsg &= " -- Error Message: " & .Message
                'incidentConsquenceType = "Unknown"
                'Throw New System.Exception("Database access error - could not determine whether consequence is processed or need to notify.")
                errorMsg &= "Database access error - could not SELECT FROM  DC_Student - getStudentData Function."
            End With
        Finally
            conn.Close()
            conn.Dispose()
            If Not errorMsg Is String.Empty Then
                Throw New System.Exception(errorMsg)
            End If
        End Try

        Return DPList
    End Function

    Private Shared Function getHOHID(studentID As String) As String
        Dim hohID As String = String.Empty
        Dim errorMsg As String = String.Empty

        Dim conn As New System.Data.SqlClient.SqlConnection
        Dim dsHOHData As New DataSet

        SqlContext.Pipe.Send("  TRY")
        Try
            conn.ConnectionString = "Context Connection=true"
            Using conn
                Dim adp As New System.Data.SqlClient.SqlDataAdapter("SELECT DC_StudentManagement_Organisation.EmployeeID FROM  DC_Student INNER JOIN DC_StudentManagement_Organisation ON DC_Student.house = DC_StudentManagement_Organisation.roleIdentifier WHERE (DC_Student.studentID = @studentID)", conn)
                adp.SelectCommand.CommandType = CommandType.Text
                adp.SelectCommand.Parameters.Add("@studentID", SqlDbType.VarChar, 50).Value = studentID

                conn.Open()
                adp.Fill(dsHOHData, "HOHData")
            End Using
            If dsHOHData.Tables(0).Rows.Count > 0 Then
                For Each row As DataRow In dsHOHData.Tables(0).Rows
                    If Not IsDBNull(row("EmployeeID")) Then
                        hohID = row("EmployeeID").ToString
                    End If
                Next
            End If
        Catch ex As SqlException
            With ex
                errorMsg = "Error Number: " & .Number
                errorMsg &= " -- Error State: " & .State
                errorMsg &= " -- Error Message: " & .Message
                'incidentConsquenceType = "Unknown"
                'Throw New System.Exception("Database access error - could not determine whether consequence is processed or need to notify.")
                errorMsg &= "Database access error - could not SELECT FROM  DC_Student - getStudentData Function."
            End With
        Finally
            conn.Close()
            'conn.Dispose()
            If Not errorMsg Is String.Empty Then
                SqlContext.Pipe.Send(errorMsg)
                Throw New System.Exception(errorMsg)
            End If

        End Try
        
        Return hohID
    End Function

    Private Shared Function getPastoralID(studentID As String) As Collection
        Dim pastoralIDList As New Collection

        Dim errorMsg As String = String.Empty

        Dim conn As New System.Data.SqlClient.SqlConnection
        Dim dsPastoralData As New DataSet

        Try
            conn.ConnectionString = "Context Connection=true"
            Using conn
                Dim adp As New System.Data.SqlClient.SqlDataAdapter("SELECT DC_StudentManagement_Organisation.EmployeeID FROM  DC_Student INNER JOIN DC_StudentManagement_Organisation ON DC_Student.pastoralClass = DC_StudentManagement_Organisation.roleIdentifier WHERE (DC_Student.studentID = @studentID)", conn)
                adp.SelectCommand.CommandType = CommandType.Text

                '@consequenceID as int
                adp.SelectCommand.Parameters.Add("@studentID", SqlDbType.VarChar, 50).Value = studentID

                conn.Open()
                adp.Fill(dsPastoralData, "PastoralData")
            End Using
            If dsPastoralData.Tables(0).Rows.Count > 0 Then
                For Each row As DataRow In dsPastoralData.Tables(0).Rows
                    If Not IsDBNull(row("EmployeeID")) Then
                        pastoralIDList.Add(row("EmployeeID"), row("EmployeeID"))
                    End If
                Next
            End If
        Catch ex As SqlException
            With ex
                errorMsg = "Error Number: " & .Number
                errorMsg &= " -- Error State: " & .State
                errorMsg &= " -- Error Message: " & .Message
                'incidentConsquenceType = "Unknown"
                'Throw New System.Exception("Database access error - could not determine whether consequence is processed or need to notify.")
                errorMsg &= "Database access error - could not SELECT FROM  DC_Student - getStudentData Function."
            End With
        Finally
            conn.Close()
            conn.Dispose()
            If Not errorMsg Is String.Empty Then
                Throw New System.Exception(errorMsg)
            End If
        End Try

        Return pastoralIDList
    End Function

    Private Shared Function getStudentData(studentID As String) As Collection
        Dim StudentData As New Collection
        Dim errorMsg As String = String.Empty

        Dim conn As New System.Data.SqlClient.SqlConnection
        Dim dsStudentData As New DataSet

        Try
            conn.ConnectionString = "Context Connection=true"
            Using conn
                Dim adp As New System.Data.SqlClient.SqlDataAdapter("SELECT * FROM DC_Student WHERE studentID=@studentID", conn)
                adp.SelectCommand.CommandType = CommandType.Text

                '@consequenceID as int
                adp.SelectCommand.Parameters.Add("@studentID", SqlDbType.VarChar, 50).Value = studentID

                conn.Open()
                adp.Fill(dsStudentData, "StudentData")
            End Using
            If dsStudentData.Tables(0).Rows.Count > 0 Then
                For Each row As DataRow In dsStudentData.Tables(0).Rows
                    For Each column In dsStudentData.Tables(0).Columns
                        If Not IsDBNull(row(column)) Then
                            Dim dataItem As TemplateDataType
                            dataItem.fieldName = column.ToString
                            dataItem.dataItem = row(column).ToString
                            StudentData.Add(dataItem, dataItem.fieldName)
                            'Dim columnName As String = column.ToString
                            'Dim columnValue As String = row(column).ToString

                            'StudentData.Add(columnValue, columnName)
                        End If
                    Next
                Next
            End If
        Catch ex As SqlException
            With ex
                errorMsg = "Error Number: " & .Number
                errorMsg &= " -- Error State: " & .State
                errorMsg &= " -- Error Message: " & .Message
                'incidentConsquenceType = "Unknown"
                'Throw New System.Exception("Database access error - could not determine whether consequence is processed or need to notify.")
                errorMsg &= "Database access error - could not SELECT FROM  DC_Student - getStudentData Function."
            End With
        Finally
            conn.Close()
            conn.Dispose()
            If Not errorMsg Is String.Empty Then
                Throw New System.Exception(errorMsg)
            End If
        End Try

        Return StudentData

    End Function

    Private Shared Function getStaffData(employeeID As String) As Collection
        Dim StaffData As New Collection
        Dim errorMsg As String = String.Empty

        Dim conn As New System.Data.SqlClient.SqlConnection
        Dim dsStaffData As New DataSet

        Try
            conn.ConnectionString = "Context Connection=true"
            Using conn
                Dim adp As New System.Data.SqlClient.SqlDataAdapter("SELECT * FROM DC_Staff WHERE employeeID=@employeeID", conn)
                adp.SelectCommand.CommandType = CommandType.Text
                adp.SelectCommand.Parameters.Add("@employeeID", SqlDbType.VarChar, 50).Value = employeeID

                conn.Open()
                adp.Fill(dsStaffData, "StaffData")
            End Using
            If dsStaffData.Tables(0).Rows.Count > 0 Then
                For Each row As DataRow In dsStaffData.Tables(0).Rows
                    For Each column In dsStaffData.Tables(0).Columns
                        If Not IsDBNull(row(column)) Then
                            Dim dataItem As TemplateDataType
                            dataItem.fieldName = column.ToString
                            dataItem.dataItem = row(column).ToString
                            StaffData.Add(dataItem, dataItem.fieldName)
                        End If
                    Next
                Next
            End If
        Catch ex As SqlException
            With ex
                errorMsg = "Error Number: " & .Number
                errorMsg &= " -- Error State: " & .State
                errorMsg &= " -- Error Message: " & .Message
                'incidentConsquenceType = "Unknown"
                'Throw New System.Exception("Database access error - could not determine whether consequence is processed or need to notify.")
                errorMsg &= "Database access error - could not SELECT FROM  DC_Staff - getStaffData Function."
            End With
        Finally
            conn.Close()
            conn.Dispose()
            If Not errorMsg Is String.Empty Then
                Throw New System.Exception(errorMsg)
            End If
        End Try

        Return StaffData

    End Function

    Private Shared Function getFamilyData(studentID As String) As Collection
        Dim FamilyData As New Collection
        Dim errorMsg As String = String.Empty

        Dim conn As New System.Data.SqlClient.SqlConnection
        Dim dsFamilyData As New DataSet

        Try
            conn.ConnectionString = "Context Connection=true"
            Using conn
                Dim adp As New System.Data.SqlClient.SqlDataAdapter("SELECT DC_Family.* FROM  DC_Student INNER JOIN DC_Family ON DC_Student.familyID = DC_Family.familyID WHERE (DC_Student.studentID = @studentID)", conn)
                adp.SelectCommand.CommandType = CommandType.Text

                '@consequenceID as int
                adp.SelectCommand.Parameters.Add("@studentID", SqlDbType.VarChar, 50).Value = studentID

                conn.Open()
                adp.Fill(dsFamilyData, "FamilyData")
            End Using
            If dsFamilyData.Tables(0).Rows.Count > 0 Then
                For Each row As DataRow In dsFamilyData.Tables(0).Rows
                    For Each column In dsFamilyData.Tables(0).Columns
                        If Not IsDBNull(row(column)) Then
                            Dim dataItem As TemplateDataType
                            dataItem.fieldName = column.ToString
                            dataItem.dataItem = row(column).ToString
                            FamilyData.Add(dataItem, dataItem.fieldName)
                        End If
                    Next
                Next
            End If
        Catch ex As SqlException
            With ex
                errorMsg = "Error Number: " & .Number
                errorMsg &= " -- Error State: " & .State
                errorMsg &= " -- Error Message: " & .Message
                'incidentConsquenceType = "Unknown"
                'Throw New System.Exception("Database access error - could not determine whether consequence is processed or need to notify.")
                errorMsg &= "Database access error - could not SELECT FROM  DC_Family - getFamilyData Function."
            End With
        Finally
            conn.Close()
            conn.Dispose()
            If Not errorMsg Is String.Empty Then
                Throw New System.Exception(errorMsg)
            End If
        End Try

        Return FamilyData

    End Function

#Region "templateData Data Definition"
    Private Structure TemplateDataType
        Public fieldName As String
        Public dataItem As String
    End Structure
#End Region

End Class
